using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Model
{
	public class UndoRedoModel
	{
		private Model _model;
		private bool _NewSelection;

		public UndoRedoModel(Model model)
		{
			_model = model;
			_NewSelection = true;

			_model.SelectionChanged += OnSelectionChanged;

			//connect(model, SIGNAL(selectionChanged()), this, SLOT(onSelectionChanged()));
		}

		private void OnSelectionChanged(object render, EventArgs e)
		{

		}

		public void Checkpoint(string str)
		{

		}

		void Undo() { }

		void Redo() { }
	}
}
