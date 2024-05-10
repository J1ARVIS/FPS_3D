using UnityEngine;
using UnityEngine.InputSystem;
using FPS.Input;
using System.Collections;

namespace FPS.Core
{
    public class Weapon : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animationTime;
        [Space]
        [Header("Firing")]
        [SerializeField] private Rigidbody _arrowPrefab;
        [SerializeField] private Transform _arrowSpawnPoint;
        [SerializeField] private float _arrowSpeed;
        [SerializeField] private Camera _cameraPlayer;

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
                //StartCoroutine(PlayFiringAnimation()); // not work
                PerformShoot();
            }
        }
        private void PerformShoot()
        {
            var screenCenterPoint = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
            var ray = _cameraPlayer.ScreenPointToRay(screenCenterPoint);
            
            if(Physics.Raycast(ray, out var hit, 50f))
            {
                var arrowDirection = (hit.point - _arrowSpawnPoint.position).normalized;
                var arrowRotation = Quaternion.LookRotation(arrowDirection);

                var arrowInstance = Instantiate(_arrowPrefab, _arrowSpawnPoint.position, arrowRotation);
                arrowInstance.AddForce(arrowDirection * _arrowSpeed, ForceMode.Impulse);
            }
        }
        private IEnumerator PlayFiringAnimation()
        {
            _animator.SetBool("Fire", true);
            _isFiring = true;

            yield return new WaitForSeconds(_animationTime);

            _animator.SetBool("Fire", false);
            _isFiring = false;
        }
    }
}


