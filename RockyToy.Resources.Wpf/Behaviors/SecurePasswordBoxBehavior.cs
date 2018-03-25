using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using JetBrains.Annotations;
using RockyToy.Contracts.Common.Extensions;

namespace RockyToy.Resources.Wpf.Behaviors
{
	/// <summary>
	///   make Password (SecureString) bindable
	/// </summary>
	public class SecurePasswordBoxBehavior : Behavior<PasswordBox>
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register("Password", typeof(SecureString), typeof(SecurePasswordBoxBehavior),
				new PropertyMetadata(OnSourcePropertyChanged));

		[UsedImplicitly]
		public SecureString Password
		{
			get => (SecureString) GetValue(PasswordProperty);
			set => SetValue(PasswordProperty, value);
		}

		protected override void OnAttached()
		{
			AssociatedObject.PasswordChanged -= OnPasswordBoxValueChanged;
			AssociatedObject.PasswordChanged += OnPasswordBoxValueChanged;
		}

		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is SecurePasswordBoxBehavior behavior))
				return;

			if (!(e.NewValue is SecureString pwd))
				return;

			if (pwd.IsEqualTo(behavior.AssociatedObject.SecurePassword))
				return;

			behavior.AssociatedObject.PasswordChanged -= OnPasswordBoxValueChanged;
			behavior.AssociatedObject.Password = pwd.ToUnsecureString();
			behavior.AssociatedObject.PasswordChanged += OnPasswordBoxValueChanged;
		}

		private static void OnPasswordBoxValueChanged(object sender, RoutedEventArgs e)
		{
			if (!(sender is PasswordBox passwordBox))
				return;

			var behavior = Interaction.GetBehaviors(passwordBox).OfType<SecurePasswordBoxBehavior>().FirstOrDefault();
			if (behavior == null)
				return;

			var binding = BindingOperations.GetBindingExpression(behavior, PasswordProperty);
			if (binding == null)
				return;

			var property = binding.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);
			if (property != null)
				property.SetValue(binding.DataItem, passwordBox.SecurePassword, null);
		}
	}
}