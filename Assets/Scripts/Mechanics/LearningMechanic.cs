using System;
using System.Collections;
using Flatformer.GameData;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanics
{
    [Serializable]
    public struct LearningStep
    {
        public Transform learningBlockSpawnPosition;
        public GameObject targetBlockForDelete;
        public LearningBuildStep learningBuildStep;
    }
    
    public class LearningMechanic : MonoBehaviour
    {
        [SerializeField] private LearningStep[] _learningSteps;
        
        private GameObject _learningBlockPrefab;
        private GameObject _activeLearningBlock;
        private int _currentStep;
        
        private void Start()
        {
            _learningBlockPrefab = GameManager.Instance.learningBlockPrefab;
            StartCoroutine(WaitForStartEditMode());
        }

        private void StartLearning()
        {
            _activeLearningBlock = Instantiate(_learningBlockPrefab);
            _activeLearningBlock.transform.position = new Vector3(0, 0, 0.9f);
            NextLearningStep();
        }

        private void OnDestroy()
        {
            Destroy(_activeLearningBlock);
            GameManager.Instance.learningBlockForStartGame.SetActive(false);
        }

        public void NextLearningStep()
        {
            if (_currentStep == _learningSteps.Length)
            {
                Destroy(_activeLearningBlock);
                GameManager.Instance.learningBlockForStartGame.SetActive(true);
                return;
            }
            _activeLearningBlock.transform.position = 
                _learningSteps[_currentStep].learningBlockSpawnPosition.position;
            if (_learningSteps[_currentStep].learningBuildStep != null)
            {
               _learningSteps[_currentStep].learningBuildStep.Init(true, this); 
            }
            else
            {
                StartCoroutine(WaifForBlockDelete(_currentStep));
            }
            _currentStep++;
        }

        private IEnumerator WaifForBlockDelete(int index)
        {
            yield return new WaitUntil(() => _learningSteps[index].targetBlockForDelete == null);
            NextLearningStep();
        }

        private IEnumerator WaitForStartEditMode()
        {
            yield return new WaitUntil(() => GameState.IsEditMode);
            Invoke(nameof(StartLearning), 0.5f);
        }
    }
}