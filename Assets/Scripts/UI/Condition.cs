using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [Header("Condition Setting")]
    public float curValue;          // ���� ��
    public float starValue;         // ���� ��
    public float maxValue;          // �ִ� ��

    public float passiverValue;     // �ڿ� ȸ�� �ӵ�
    public Image uiGauge;           // UI ������ �̹���

    void Start()
    {
        // ���� ���� ���� ������ ����
        curValue = starValue;
    }

    void Update()
    {
        // UI �������� fillAmount�� ���� ���� ������� ����
        uiGauge.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        // ���� ���� �ִ� ���� ������ ��ȯ
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        // �� ����, �� �ִ� ���� �ʰ����� �ʵ��� ����
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        // �� ����, �� �ּ� ���� 0 ���Ϸ� �������� �ʵ��� ����
        curValue = Mathf.Max(curValue - value, 0.0f);
    }
}
