using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    // 화톳불이 주는 피해량
    public float damage;

    // 피해를 주는 주기 (초 단위)
    public float damageRate;

    // 화톳불 범위 내에 있는 피해를 받을 수 있는 객체 목록
    public List<IDamageable> things = new List<IDamageable>();

    void Start()
    {
        // 일정 간격마다 DealDamage() 실행 (damageRate 초마다 실행)
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    /// <summary>
    /// 범위 내의 모든 객체에 피해를 줌
    /// </summary>
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakeDamage(damage); // 객체에 피해를 적용
        }
    }

    /// <summary>
    /// 화톳불 범위에 들어온 객체를 목록에 추가
    /// </summary>
    /// <param name="other">트리거에 감지된 콜라이더</param>
    private void OnTriggerEnter(Collider other)
    {
        // 들어온 객체가 IDamageable을 구현하고 있다면 목록에 추가
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    /// <summary>
    /// 화톳불 범위에서 벗어난 객체를 목록에서 제거
    /// </summary>
    /// <param name="other">트리거에서 벗어난 콜라이더</param>
    private void OnTriggerExit(Collider other)
    {
        // 나간 객체가 IDamageable을 구현하고 있다면 목록에서 제거
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
