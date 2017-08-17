using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAtaque : StateMachineBehaviour
{
    public enum Tipo{Melee, Range};
    public Tipo tipo = Tipo.Melee;

    public enum DireccionImpacto {Adelante,Atras,Arriba,Abajo};
    public DireccionImpacto dirImpacto;
    public enum FuerzaImpacto {Debil, Medio,Fuerte}
    public FuerzaImpacto fImpacto;

    public Vector2 GetImpacto()
    {
        return new Vector2((int)dirImpacto,(int)fImpacto);
    }

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
