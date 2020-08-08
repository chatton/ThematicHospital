using System;
using UnityEngine;

namespace Animations
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        
    }
}