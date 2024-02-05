using UnityEngine;
using UnityEngine.UI;

namespace Bonfire
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] Text debugText = null;


        // 体力
        public int HP = 100;
        public int MaxHP = 100;

        // 攻撃力
        public int Power = 10;

        // 状態
        public bool isGrounded = false;
        public bool isAnimationEnd = true;
        public bool isLockOn = false;

        public bool isAttackR1 = false;
        public bool isAttackR1_2 = false;
        public bool isRolling = false;
        public bool isJumping = false;
        public bool isDied = false;
        public bool EquipWepon = false;
        public bool UnequipWeapon = false;

        public void resetStatus()
        {
            isAnimationEnd = true;

            isAttackR1 = false;
            isAttackR1_2 = false;
            isRolling = false;
            isJumping = false;
            isDied = false;
            EquipWepon = false;
            UnequipWeapon = false;
        }

        private void Update()
        {
            if(debugText != null) 
            {
                debugText.text = string.Format ("HP = {0}/{1}", HP, MaxHP);
            }
        }
    }
}
