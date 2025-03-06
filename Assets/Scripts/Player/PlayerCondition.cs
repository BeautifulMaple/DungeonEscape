using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCondition : MonoBehaviour
{
    public UICondition uICondition; // UI���� ���� ������ �����ϴ� ��ü

    // UICondition���� ü�°� ���¹̳� ���¸� ������
    Condition health { get { return uICondition.health; } }
    Condition stamina { get { return uICondition.Stamina; } }

    public event Action OnTakeDamaged;  // �������� �޾��� �� �߻��ϴ� �̺�Ʈ

    void Update()
    {
        // ���¹̳ʰ� �ð��� ���� �ڿ� ȸ���ǵ��� ����
        stamina.Add(stamina.passiverValue * Time.deltaTime);
    }

    public void Heal(float amount)
    {
        // ü���� ȸ��
        health.Add(amount);
    }

    private void Die()
    {
        // �÷��̾� ��� ó�� (����� �α� ���)
        Debug.Log($"Die");
    }
    public void TakeDamage(float damage)
    {
        // �������� ������ ü���� ����
        health.Subtract(damage);
        // �������� �޾Ҵٴ� �̺�Ʈ �߻�
        OnTakeDamaged?.Invoke();
    }
    public bool UseStamina(float amount)
    {
        // ���¹̳ʰ� �����ϸ� ��� �Ұ� ó��
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        // ���¹̳� ����
        stamina.Subtract(amount);
        return true;
    }
}
