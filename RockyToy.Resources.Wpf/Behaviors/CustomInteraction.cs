using System.Windows;
using System.Windows.Interactivity;

namespace RockyToy.Resources.Wpf.Behaviors
{
	/// <summary>
	///   ref: https://stackoverflow.com/questions/1647815/how-to-add-a-blend-behavior-in-a-style-setter
	/// </summary>
	public class CustomInteraction
	{
		public static readonly DependencyProperty BehaviorsProperty =
			DependencyProperty.RegisterAttached("Behaviors", typeof(Behaviors), typeof(CustomInteraction),
				new UIPropertyMetadata(null, OnPropertyBehaviorsChanged));

		public static readonly DependencyProperty TriggersProperty =
			DependencyProperty.RegisterAttached("Triggers", typeof(Triggers), typeof(CustomInteraction),
				new UIPropertyMetadata(null, OnPropertyTriggersChanged));

		public static readonly DependencyProperty InputBindingsProperty =
			DependencyProperty.RegisterAttached("InputBindings", typeof(InputBindings), typeof(CustomInteraction),
				new UIPropertyMetadata(null, OnPropertyInputBindingsChanged));

		public static Behaviors GetBehaviors(DependencyObject obj)
		{
			return (Behaviors) obj.GetValue(BehaviorsProperty);
		}

		public static void SetBehaviors(DependencyObject obj, Behaviors value)
		{
			obj.SetValue(BehaviorsProperty, value);
		}

		private static void OnPropertyBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behaviors = Interaction.GetBehaviors(d);
			if (!(e.NewValue is Behaviors newValue))
				return;
			foreach (var behavior in newValue)
				behaviors.Add(behavior);
		}

		public static Triggers GetTriggers(DependencyObject obj)
		{
			return (Triggers) obj.GetValue(TriggersProperty);
		}

		public static void SetTriggers(DependencyObject obj, Triggers value)
		{
			obj.SetValue(TriggersProperty, value);
		}

		private static void OnPropertyTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var triggers = Interaction.GetTriggers(d);
			if (!(e.NewValue is Triggers newValue))
				return;
			foreach (var trigger in newValue)
				triggers.Add(trigger);
		}

		public static InputBindings GetInputBindings(DependencyObject obj)
		{
			return (InputBindings) obj.GetValue(InputBindingsProperty);
		}

		public static void SetInputBindings(DependencyObject obj, InputBindings value)
		{
			obj.SetValue(InputBindingsProperty, value);
		}

		private static void OnPropertyInputBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is UIElement ele))
				return;
			if (!(e.NewValue is InputBindings newValue))
				return;

			ele.InputBindings.AddRange(newValue);
		}
	}
}