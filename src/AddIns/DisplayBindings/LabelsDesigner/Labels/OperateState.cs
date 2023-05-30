using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Labels
{
	public enum OperateState
	{
		IdleState,
		ArrowSelectRegion,
		ArrowMove,
		ArrowResize,
		CreateIdle,
		CreateDrag
	};
}
