using Stateless;
using System;

namespace LearnStateless.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var carState = new StateMachine<CarState, CarTrigger>(CarState.Locked);

            carState.Configure(CarState.Locked).Permit(CarTrigger.Unlock, CarState.Unlocked);
            carState.Configure(CarState.Unlocked).Permit(CarTrigger.Start, CarState.NotMoving).Permit(CarTrigger.Lock, CarState.Locked);
            carState.Configure(CarState.NotMoving).Permit(CarTrigger.Go, CarState.Moving).Permit(CarTrigger.Stop, CarState.Unlocked);
            carState.Configure(CarState.Moving).Permit(CarTrigger.Break, CarState.NotMoving);
            carState.OnUnhandledTrigger((s, t) =>
            {
                Console.WriteLine("Trigger {0} cannot be used in state {1}", t, s);
            });


            var printStateAction = new Action(() => Console.WriteLine("State: {0}", carState.State));
            var printTriggerAction = new Action<CarTrigger>(t => Console.WriteLine("Applying Trigger: {0}", t));
            var applyTriggerAction = new Action<CarTrigger>(t =>
            {
                printTriggerAction(t);
                carState.Fire(t);
                printStateAction();
            });

            // Print initial state.
            printStateAction();

            // Apply triggers.
            applyTriggerAction(CarTrigger.Unlock);
            applyTriggerAction(CarTrigger.Start);
            applyTriggerAction(CarTrigger.Go);
            applyTriggerAction(CarTrigger.Break);
            applyTriggerAction(CarTrigger.Stop);
            applyTriggerAction(CarTrigger.Lock);
        }
    }
}
