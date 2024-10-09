using System;
using System.Collections;
using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

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
            if(!YandexGame.savesData.isNeedLearning) return;
            GameManager.Instance.startGameButton.SetActive(false);
            _learningBlockPrefab = GameManager.Instance.learningBlockPrefab;
            StartCoroutine(WaitForStartEditMode());
        }

        private void OnEnable()
        {
            EventDispatcherExtension.RegisterListener(EventID.EditMode, param =>
            {
                if(!(bool)param)DisableLearning();
            });
        }

        private void DisableLearning()
        {
            if(_activeLearningBlock == null) return;
            _activeLearningBlock.SetActive(false);
        }

        private void StartLearning()
        {
            _activeLearningBlock = Instantiate(_learningBlockPrefab);
            _activeLearningBlock.transform.position = new Vector3(0, 0, 0.9f);
            NextLearningStep();
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EventID.EditMode, param =>
            {
                if(!(bool)param)DisableLearning();
            });
            Destroy(_activeLearningBlock);
            GameManager.Instance.learningBlockForStartGame.SetActive(false);
        }

        public void NextLearningStep()
        {
            if (_currentStep == _learningSteps.Length)
            {
                Destroy(_activeLearningBlock);
                GameManager.Instance.learningBlockForStartGame.SetActive(true);
                GameManager.Instance.startGameButton.SetActive(true);
                return;
            }

            _activeLearningBlock.transform.rotation = Quaternion.identity;
            _activeLearningBlock.transform.parent = _learningSteps[_currentStep].learningBlockSpawnPosition.transform;
            _activeLearningBlock.transform.localPosition = Vector3.zero;
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