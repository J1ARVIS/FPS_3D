using UnityEngine;
using UnityEngine.InputSystem;
using FPS.Input;
using System.Collections;

namespace FPS.Core
{
    public class CrossbowController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _firingTime;

        private InputControlerMain _inputControler;

        private bool _isFiring = false;

        private void Awake()
        {
            _inputControler = new InputControlerMain();
            _inputControler.Player.Fire.performed += Fire;
        }
        private void Fire(InputAction.CallbackContext context)
        {
            if (!_isFiring)
            {
                StartCoroutine(PlayFiringAnimation());
            }
        }
        private IEnumerator PlayFiringAnimation()
        {
            _animator.SetBool("Fire", true);
            _isFiring = true;

            yield return new WaitForSeconds(_firingTime);

            _animator.SetBool("Fire", false);
            _isFiring = false;
        }
    }
}


