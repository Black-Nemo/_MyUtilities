using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EatableItem", menuName = "Item/EatableItem")]
public class EatableItem : Item, IUseable
{
    public string EAT_ANIMATION_NAME;
    public float CoolDownTime;

    public AudioClip[] NamNamNam;


    private bool _isAttack;
    private bool _isTouch;

    private bool _changedItem;
    public void Down(Character character)
    {
        _isTouch = true;
        //if (!character.HealthSystem.IsFullHealth())
        //{
        GameManager.Instance.StartCoroutine(EatEnumerator(character, this, CoolDownTime));
        
        GameManager.Instance.LocalCharacter.characterSelectedItemManager.ChangeSelectedItemEvent += ChangeSelectedItem;
        _changedItem = false;
        //}
        //else
        //{
        //    TextWorldCanvas textWorldCanvas = Instantiate(GameRememberObjectManager.Instance.TextWorldCanvasPrefab, character.transform.position + ((Camera.main.transform.position - character.transform.position).normalized * 3) + Vector3.up * 1.5f, Quaternion.identity).GetComponent<TextWorldCanvas>();
        //    textWorldCanvas.textMeshProUGUI.text = "<color=#DE8F5F>Canın Dolu</color>";
        //}
    }

    public void Up(Character character)
    {
        _isTouch = false;
        GameManager.Instance.LocalCharacter.characterSelectedItemManager.ChangeSelectedItemEvent -= ChangeSelectedItem;
        _changedItem = false;
    }

    private IEnumerator EatEnumerator(Character character, EatableItem eatableItem, float coolDownTime)
    {
        AudioSource audioSource = SoundsManager.Instance.InstantiateSound(eatableItem.NamNamNam[UnityEngine.Random.Range(0, eatableItem.NamNamNam.Length)], character.transform.position, 5);
        character.CharacterAttackSystem.AttackEvent?.Invoke(eatableItem.EAT_ANIMATION_NAME);

        _isAttack = true;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(coolDownTime / 10);
            if (!_isTouch || _changedItem) { character.CharacterAttackSystem.AttackEvent?.Invoke("Def"); _isAttack = false; Destroy(audioSource.gameObject); _changedItem = false; yield break; }
        }

        _isAttack = false;
        if (_isTouch)
        {
            foreach (var item in features)
            {
                item.AddFeature(character);
            }
            character.selectedSlot.Slot.RemoveItem(1);
            character.InventoryOfTheObjectHotbar.Inventory.ChangeValues();
            //character.RemoveItem(eatableItem, 1);
        }
    }

    public void ChangeSelectedItem()
    {
        _changedItem = true;
    }

    public override string GetInfo()
    {
        return GetFeaturesInfo();
    }
}