using System;
using System.Collections.Generic;

namespace State
{
    public class StateMachine
    {
        private IState CurrentState;
        private readonly Dictionary<Type, List<Transition>> Transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> CurrentTransitions = new List<Transition>();
        private readonly List<Transition> AnyTransitions = new List<Transition>();
        private static readonly List<Transition> EmptyTransitions = new List<Transition>();

        public void Tick(float deltaTime)
        {
            Transition transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.To);
            }

            CurrentState?.Tick(deltaTime);
        }

        public void SetState(IState state)
        {
            if (state == CurrentState)
            {
                return;
            }

            CurrentState?.OnExit();
            CurrentState = state;

            Transitions.TryGetValue(CurrentState.GetType(), out CurrentTransitions);
            if (CurrentTransitions == null)
            {
                CurrentTransitions = EmptyTransitions;
            }


            CurrentState?.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (!Transitions.TryGetValue(from.GetType(), out List<Transition> transitions))
            {
                transitions = new List<Transition>();
                Transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }


        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            AnyTransitions.Add(new Transition(state, predicate));
        }

        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            foreach (Transition transition in AnyTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            foreach (Transition transition in CurrentTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            return null;
        }
    }
}