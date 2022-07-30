using UnityEngine;

// ĳ���� �ִϸ��̼� ��Ʈ�ѷ�
public class AnimationController : MonoBehaviour
{
    // �ִϸ�����
    Animator anim;

    // �ִϸ��̼� ���� ����
    bool canAnim = true;

    // �ִϸ��̼� Ȱ��ȭ ����
    // toggle = ture �ִϸ��̼� Ȱ��ȭ
    // toggle = false �ִϸ��̼� ��Ȱ��ȭ �� ��� ����
    public void OnAnimator(bool toggle)
    {
        canAnim = toggle;

        if (canAnim == false)
        {
            anim.ResetTrigger("onJump");
            anim.Play("Wait");
        }
    }

    // ���� �ִϸ��̼� ����
    public void PlayJump()
    {
        if (canAnim)
            anim.SetTrigger("onJump");
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.Log("[���] �ִϸ����Ͱ� �������� �ʽ��ϴ�");
    }
}