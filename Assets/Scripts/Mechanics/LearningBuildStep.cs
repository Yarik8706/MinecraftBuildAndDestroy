using System;
using UnityEngine;

namespace Mechanics
{
    public class LearningBuildStep : MonoBehaviour
    {
        private bool _isActiveStep;
        private LearningMechanic _learningMechanic;

        public void Init(bool isActiveStep, LearningMechanic learningMechanic)
        {
            _isActiveStep = isActiveStep;
            _learningMechanic = learningMechanic;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActiveStep && other.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                _learningMechanic.NextLearningStep();
                Destroy(gameObject);
            }
        }
    }
}