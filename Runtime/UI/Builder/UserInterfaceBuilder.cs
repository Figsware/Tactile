using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Tactile.UI.Builder
{
    [AddComponentMenu("Tactile/UI/User Interface Builder")]
    public class UserInterfaceBuilder: MonoBehaviour
    {
        [SerializeField] private RectTransform buildTransform;
        protected IUserInterface _userInterface;
        
        private void Start()
        {
            BuildUserInterface();   
        }

        public void SetUserInterface(IUserInterface userInterface)
        {
            if (_userInterface != null)
            {
                _userInterface.OnViewUpdated -= BuildUserInterface;
            }

            if (userInterface != null)
            {
                userInterface.OnViewUpdated += BuildUserInterface;
            }
            
            _userInterface = userInterface;
            BuildUserInterface();
        }
        
        public void BuildUserInterface()
        {
            if (_userInterface != null)
            {
                _userInterface.Build(buildTransform);
            }
        }
    }
}