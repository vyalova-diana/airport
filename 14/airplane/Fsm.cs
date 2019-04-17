using System;

namespace airplane
{
	public class Fsm
	{
		public Action ActiveState { get; private set; } // points to the currently active state function

		public Fsm() { }

		public void SetState(Action state)
		{
			ActiveState = state;
		}

		public void Update()
		{
			ActiveState?.Invoke();
		}
	}
}
