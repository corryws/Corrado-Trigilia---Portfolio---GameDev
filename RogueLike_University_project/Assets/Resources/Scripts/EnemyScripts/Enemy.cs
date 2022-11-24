using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*QUESTO SCRIPT E' PARTICOLARE POICHE' NON VIENE ASSEGNATO AD UN OGGETTO MA
BENSI MI PERMETTE DI CREARE DIRETTAMENTE NELLA HIERARCHY DEI "SCRIPTABLE OBJECT"
OVVERO DEGLI OGGETTI CHE "NASCONO" GIA' IMPOSTATI CON QUESTO SCRIPT E QUINDI
E' POSSIBILE ASSEGNARE AD OGNUNO DI ESSI DEI PARAMETRI DIVERSI
QUESTO METODO E' MOLTO EFFICACE PERCHE' CI PERMETTE DI EVITARE LA CREAZIONE
DI DATABASE(FAKE) MEDIANTE ARRAY O LISTE*/
[CreateAssetMenu(fileName = "New Enemy", menuName="Enemy")]
public class Enemy : ScriptableObject
{
      public AnimatorOverrideController ThisEnemyAnimator;
      public int life;
      public float dropprate,speed,shootspeed,dps,nextshoot,attackrange;
      public AudioClip verseclip;
}
