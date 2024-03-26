using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ROGUETURN, PRIESTTURN, WIZARDTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public int i = 0;
    public GameObject playerPrefab;
    public GameObject roguePrefab;
    public GameObject priestPrefab;
    public GameObject wizardPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform rogueBattleStation;
    public Transform priestBattleStation;
    public Transform wizardBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit rogueUnit;
    Unit priestUnit;
    Unit wizardUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI specialText;

    public BattleHud playerHud;
    public BattleHud rogueHud;
    public BattleHud priestHud;
    public BattleHud wizardHud;
    public BattleHud enemyHud;

    public GameObject attackButton;
    public GameObject specialButton;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject rogueGO = Instantiate(roguePrefab, rogueBattleStation);
        rogueUnit = rogueGO.GetComponent<Unit>();

        GameObject priestGO = Instantiate(priestPrefab, priestBattleStation);
        priestUnit = priestGO.GetComponent<Unit>();

        GameObject wizardGO = Instantiate(wizardPrefab, wizardBattleStation);
        wizardUnit = wizardGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        attackButton.SetActive(false);
        specialButton.SetActive(false);

        dialogueText.text = "A crazy " + enemyUnit.unitName + " approaches...";

        playerHud.SetHUD(playerUnit);
        rogueHud.SetHUD(rogueUnit);
        priestHud.SetHUD(priestUnit);
        wizardHud.SetHUD(wizardUnit);
        enemyHud.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "Returning to main menu!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            yield return new WaitForSeconds(2f);
            dialogueText.text = "Returning to main menu!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnAttackButton()
    {
        //if (state != BattleState.PLAYERTURN || state != BattleState.ROGUETURN || state != BattleState.PRIESTTURN || state != BattleState.WIZARDTURN)
        //    return;

        StartCoroutine(PlayerAttack());
    }

    public void OnSpecialButton()
    {
        //if (state != BattleState.PLAYERTURN || state != BattleState.ROGUETURN || state != BattleState.PRIESTTURN || state != BattleState.WIZARDTURN)
        //    return;

        StartCoroutine(PlayerSpecial());
    }

    IEnumerator EnemyTurn()
    {
        int target = 0;

        if(playerUnit.CheckHP() == false)
        {
            dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName;
            target = 1;
        }
        else if(rogueUnit.CheckHP() == false)
        {
            dialogueText.text = enemyUnit.unitName + " attacks " + rogueUnit.unitName;
            target = 2;
        }
        else if(priestUnit.CheckHP() == false)
        {
            dialogueText.text = enemyUnit.unitName + " attacks " + priestUnit.unitName;
            target = 3;
        }
        else if(wizardUnit.CheckHP() == false)
        {
            dialogueText.text = enemyUnit.unitName + " attacks " + wizardUnit.unitName;
            target = 4;
        }

        yield return new WaitForSeconds(1f);

        if(target==1)
        {
            playerUnit.TakeDamage(enemyUnit.damage);
            playerHud.SetHP(playerUnit.currentHP);
        } 
        else if(target==2)
        {
            rogueUnit.TakeDamage(enemyUnit.damage);
            rogueHud.SetHP(rogueUnit.currentHP);
        }
        else if(target==3)
        {
            priestUnit.TakeDamage(enemyUnit.damage);
            priestHud.SetHP(priestUnit.currentHP);
        }
        else if(target==4)
        {
            wizardUnit.TakeDamage(enemyUnit.damage);
            wizardHud.SetHP(wizardUnit.currentHP);
        }
        
        //bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        yield return new WaitForSeconds(1f);

        if(playerUnit.CheckHP() == true && rogueUnit.CheckHP() == true && priestUnit.CheckHP() == true && wizardUnit.CheckHP() == true)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        if(playerUnit.CheckHP() == false)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Axe";
            specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Self Heal";
            attackButton.SetActive(true);
            specialButton.SetActive(true);

            dialogueText.text = "What will Warrior do?";
        }
        else
        {
            state = BattleState.ROGUETURN;
            RogueTurn();
        }
    }

    void RogueTurn()
    {
        if(rogueUnit.CheckHP() == false)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Backstab";
            specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Blood Curse";
            attackButton.SetActive(true);
            specialButton.SetActive(true);

            dialogueText.text = "What will Rogue do?";
        }
        else
        {
            state = BattleState.PRIESTTURN;
            PriestTurn();
        }
    }

    void PriestTurn()
    {
        if(priestUnit.CheckHP() == false)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mace";
            specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Healing Circle";
            attackButton.SetActive(true);
            specialButton.SetActive(true);

            dialogueText.text = "What will Priest do?";
        }
        else
        {
            state = BattleState.WIZARDTURN;
            WizardTurn();
        }
    }

    void WizardTurn()
    {
        if(wizardUnit.CheckHP() == false)
        {
            attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Magic Missile";
            specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fireball";
            attackButton.SetActive(true);
            specialButton.SetActive(true);

            dialogueText.text = "What will Wizard do?";
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator PlayerAttack()
    {
        attackButton.SetActive(false);
        specialButton.SetActive(false);

        bool isDead = false;

        if(state == BattleState.PLAYERTURN)
        {
            isDead = enemyUnit.TakeDamage(playerUnit.damage);
        } 
        else if(state == BattleState.ROGUETURN)
        {
            isDead = enemyUnit.TakeDamage(rogueUnit.damage);
        }
        else if(state == BattleState.PRIESTTURN)
        {
            isDead = enemyUnit.TakeDamage(priestUnit.damage);
        }
        else if(state == BattleState.WIZARDTURN)
        {
            isDead = enemyUnit.TakeDamage(wizardUnit.damage);
        }

        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            if(state == BattleState.PLAYERTURN)
            {
                state = BattleState.ROGUETURN;
                RogueTurn();
            } 
            else if(state == BattleState.ROGUETURN)
            {
                state = BattleState.PRIESTTURN;
                PriestTurn();
            }
            else if(state == BattleState.PRIESTTURN)
            {
                state = BattleState.WIZARDTURN;
                WizardTurn();
            }
            else if(state == BattleState.WIZARDTURN)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator PlayerSpecial()
    {
        attackButton.SetActive(false);
        specialButton.SetActive(false);

        if(state == BattleState.PLAYERTURN)
        {
            playerUnit.Heal(2);
            playerHud.SetHP(playerUnit.currentHP);
            dialogueText.text = "You feel renewed strength!";

            yield return new WaitForSeconds(2f);

            state = BattleState.ROGUETURN;
            RogueTurn();
        } 
        else if(state == BattleState.ROGUETURN)
        {
            rogueUnit.RecklessAttack(1);
            bool isDead = enemyUnit.TakeDamage(rogueUnit.damage*2);
            
            rogueHud.SetHP(rogueUnit.currentHP);
            enemyHud.SetHP(enemyUnit.currentHP);
            dialogueText.text = "You took some damage but the attack is successful!";

            yield return new WaitForSeconds(2f);

            if(isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PRIESTTURN;
                PriestTurn();
            }
        }
        else if(state == BattleState.PRIESTTURN)
        {
            playerUnit.Heal(1);
            rogueUnit.Heal(1);
            priestUnit.Heal(1);
            wizardUnit.Heal(1);
            playerHud.SetHP(playerUnit.currentHP);
            rogueHud.SetHP(rogueUnit.currentHP);
            priestHud.SetHP(priestUnit.currentHP);
            wizardHud.SetHP(wizardUnit.currentHP);
            dialogueText.text = "Your party feels renewed strength!";
            
            yield return new WaitForSeconds(2f);

            state = BattleState.WIZARDTURN;
            WizardTurn();
        }
        else if(state == BattleState.WIZARDTURN)
        {
            playerUnit.RecklessAttack(2);
            rogueUnit.RecklessAttack(2);
            priestUnit.RecklessAttack(2);
            wizardUnit.RecklessAttack(2);
            bool isDead = enemyUnit.TakeDamage(wizardUnit.damage*4);
            
            playerHud.SetHP(playerUnit.currentHP);
            rogueHud.SetHP(rogueUnit.currentHP);
            priestHud.SetHP(priestUnit.currentHP);
            wizardHud.SetHP(wizardUnit.currentHP);
            enemyHud.SetHP(enemyUnit.currentHP);
            dialogueText.text = "Your party took some damage but the attack is successful!";

            yield return new WaitForSeconds(2f);

            if(isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }

}
