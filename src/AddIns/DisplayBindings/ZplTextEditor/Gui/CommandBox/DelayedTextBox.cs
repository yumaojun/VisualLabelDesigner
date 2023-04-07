using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualLabelDesigner.ZplTextEditor.Gui
{
	public class DelayedTextBox : TextBox
	{
		public event EventHandler DelayedTextChanged;
		public DelayedTextBox()
		{
			this.DelayedTextChangedTimeout = 200;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.m_delayedTextChangedTimer != null)
			{
				this.m_delayedTextChangedTimer.Stop();
				if (disposing)
				{
					this.m_delayedTextChangedTimer.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		public int DelayedTextChangedTimeout { get; set; }

		protected virtual void OnDelayedTextChanged(EventArgs e)
		{
			if (this.DelayedTextChanged != null)
			{
				this.DelayedTextChanged(this, e);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			this.InitializeDelayedTextChangedEvent();
			base.OnTextChanged(e);
		}

		private void InitializeDelayedTextChangedEvent()
		{
			if (this.m_delayedTextChangedTimer != null)
			{
				this.m_delayedTextChangedTimer.Stop();
			}
			if (this.m_delayedTextChangedTimer == null || this.m_delayedTextChangedTimer.Interval != this.DelayedTextChangedTimeout)
			{
				this.m_delayedTextChangedTimer = new Timer();
				this.m_delayedTextChangedTimer.Tick += this.HandleDelayedTextChangedTimerTick;
				this.m_delayedTextChangedTimer.Interval = this.DelayedTextChangedTimeout;
			}
			this.m_delayedTextChangedTimer.Start();
		}

		private void HandleDelayedTextChangedTimerTick(object sender, EventArgs e)
		{
			(sender as Timer).Stop();
			this.OnDelayedTextChanged(EventArgs.Empty);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
		}

		private Timer m_delayedTextChangedTimer;

		private IContainer components;
	}
}
