﻿namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// Code39Ext
	/// </summary>
	public class BarcodeCode39Ext : BarcodeCode39
	{
		/* Code 39Ext ASCII map. */
		static readonly string[] asciiMap = {
			/* NUL */ "%U",   /* SOH */ "$A",   /* STX */ "$B",   /* ETX */ "$C",
			/* EOT */ "$D",   /* ENQ */ "$E",   /* ACK */ "$F",   /* BEL */ "$G",
			/* BS  */ "$H",   /* HT  */ "$I",   /* LF  */ "$J",   /* VT  */ "$K",
			/* FF  */ "$L",   /* CR  */ "$M",   /* SO  */ "$N",   /* SI  */ "$O",
			/* DLE */ "$P",   /* DC1 */ "$Q",   /* DC2 */ "$R",   /* DC3 */ "$S",
			/* DC4 */ "$T",   /* NAK */ "$U",   /* SYN */ "$V",   /* ETB */ "$W",
			/* CAN */ "$X",   /* EM  */ "$Y",   /* SUB */ "$Z",   /* ESC */ "%A",
			/* FS  */ "%B",   /* GS  */ "%C",   /* RS  */ "%D",   /* US  */ "%E",
			/* " " */ " ",    /* !   */ "/A",   /* "   */ "/B",   /* #   */ "/C",
			/* $   */ "/D",   /* %   */ "/E",   /* &   */ "/F",   /* '   */ "/G",
			/* (   */ "/H",   /* )   */ "/I",   /* *   */ "/J",   /* +   */ "/K",
			/* ,   */ "/L",   /* -   */ "-",    /* .   */ ".",    /* /   */ "/O",
			/* 0   */ "0",    /* 1   */ "1",    /* 2   */ "2",    /* 3   */ "3",
			/* 4   */ "4",    /* 5   */ "5",    /* 6   */ "6",    /* 7   */ "7",
			/* 8   */ "8",    /* 9   */ "9",    /* :   */ "/Z",   /* ;   */ "%F",
			/* <   */ "%G",   /* =   */ "%H",   /* >   */ "%I",   /* ?   */ "%J",
			/* @   */ "%V",   /* A   */ "A",    /* B   */ "B",    /* C   */ "C",
			/* D   */ "D",    /* E   */ "E",    /* F   */ "F",    /* G   */ "G",
			/* H   */ "H",    /* I   */ "I",    /* J   */ "J",    /* K   */ "K",
			/* L   */ "L",    /* M   */ "M",    /* N   */ "N",    /* O   */ "O",
			/* P   */ "P",    /* Q   */ "Q",    /* R   */ "R",    /* S   */ "S",
			/* T   */ "T",    /* U   */ "U",    /* V   */ "V",    /* W   */ "W",
			/* X   */ "X",    /* Y   */ "Y",    /* Z   */ "Z",    /* [   */ "%K",
			/* \   */ "%L",   /* ]   */ "%M",   /* ^   */ "%N",   /* _   */ "%O",
			/* `   */ "%W",   /* a   */ "+A",   /* b   */ "+B",   /* c   */ "+C",
			/* d   */ "+D",   /* e   */ "+E",   /* f   */ "+F",   /* g   */ "+G",
			/* h   */ "+H",   /* i   */ "+I",   /* j   */ "+J",   /* k   */ "+K",
			/* l   */ "+L",   /* m   */ "+M",   /* n   */ "+N",   /* o   */ "+O",
			/* p   */ "+P",   /* q   */ "+Q",   /* r   */ "+R",   /* s   */ "+S",
			/* t   */ "+T",   /* u   */ "+U",   /* v   */ "+V",   /* w   */ "+W",
			/* x   */ "+X",   /* y   */ "+Y",   /* z   */ "+Z",   /* {   */ "%P",
			/* |   */ "%Q",   /* }   */ "%R",   /* ~   */ "%S",   /* DEL */ "%T"
		};

		public static new Barcode Create()
		{
			return new BarcodeCode39Ext();
		}

		protected override bool Validate(string rawData)
		{
			foreach (char c in rawData)
			{
				if ((c < 0) || (c > 0x7F))
				{
					return false;
				}
			}

			return true;
		}

		protected override string Preprocess(string rawData)
		{
			string cookedData = string.Empty;

			foreach (char c in rawData)
			{
				cookedData += asciiMap[c];
			}

			return cookedData;
		}

		protected override string PrepareText(string rawData)
		{
			return rawData;
		}
	}
}