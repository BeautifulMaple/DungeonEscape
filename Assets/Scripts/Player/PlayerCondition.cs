using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCondition : MonoBehaviour
{
    public UICondition uICondition; // UI���� ���� ������ �����ϴ� ��ü

    // UICondition���� ü�°� ���¹̳� ���¸� ������
    Condition health { get { return uICondition.health; } }
    Condition hunger { get { return uICondition.health; } }
    Condition stamina { get { return uICondition.Stamina; } }

    public float noHunger;  //  ������� ���� �� ü�� ����
    public event Action OnTakeDamaged;  // �������� �޾��� �� �߻��ϴ� �̺�Ʈ

    void Update()
    {
        hunger.Subtract(hunger.passiverValue * Time.deltaTime);     // �ð��� ���� ��� ����
        stamina.Add(stamina.passiverValue * Time.deltaTime);    // ���¹̳ʰ� �ð��� ���� ȸ��
        if (health.curValue <= 0f)
        {
            // ������� ������ ü�� ����
            health.Subtract(noHunger * Time.deltaTime);
        }
        else if (health.curValue < 0f)
        {
            Die();

        }
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

    public void Eat(float amount)
    {
        hunger.Add(amount);
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
