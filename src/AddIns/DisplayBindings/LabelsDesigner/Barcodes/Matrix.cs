using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YProgramStudio.LabelsDesigner.Barcodes
{
	/// <summary>
	/// 二维码阵列
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Matrix<T>
	{
		/**
		 * Matrix Private data
		 */
		private int _nx;
		private int _ny;
		private T[,] _data;

		public int Nx { get => _nx; }
		public int Ny { get => _ny; }
		public T[,] Data { get => _data; }

		/// <summary>
		/// 返回二维数组的值
		/// </summary>
		/// <param name="row">row行</param>
		/// <param name="col">col列</param>
		/// <returns></returns>
		public T this[int row, int col]
		{
			get { return _data[row, col]; }
			set { _data[row, col] = value; }
		}

		public Matrix()
		{
			_nx = 0;
			_ny = 0;
			_data = default;
		}

		public Matrix(int nx, int ny)
		{
			_nx = nx;
			_ny = ny;
			_data = (nx > 0 && ny > 0) ? new T[ny, nx] : default;
		}

		public Matrix(Matrix<T> src)
		{
			_nx = src._nx;
			_ny = src._ny;
			_data = ((src._nx > 0 && src._ny > 0) ? new T[src._ny, src._nx] : default);

			for (int iy = 0; iy < _ny; iy++)
			{
				for (int ix = 0; ix < _nx; ix++)
				{
					_data[iy, ix] = src._data[iy, ix];
				}
			}
		}

		public Matrix(Matrix<T> src, int x0, int y0, int nx, int ny)
		{
			_nx = src._nx;
			_ny = src._ny;
			_data = ((nx > 0 && ny > 0) ? new T[ny, nx] : default);

			for (int iy = 0; iy < _ny; iy++)
			{
				if ((y0 + iy) < src._ny)
				{
					for (int ix = 0; ix < _nx; ix++)
					{
						if ((x0 + ix) < src._nx)
						{
							_data[iy, ix] = src._data[(y0 + iy), (x0 + ix)];
						}
					}
				}
			}
		}

		public void Resize(int nx, int ny)
		{
			_nx = nx;
			_ny = ny;
			_data = (nx > 0 && ny > 0) ? new T[ny, nx] : default;
		}

		public Matrix<T> SubMatrix(int x0, int y0, int nx, int ny)
		{
			return new Matrix<T>(this, x0, y0, nx, ny);
		}

		public void SetSubMatrix(int x0, int y0, Matrix<T> a)
		{
			for (int iy = 0; iy < a._ny; iy++)
			{
				if ((y0 + iy) < _ny)
				{
					for (int ix = 0; ix < a._nx; ix++)
					{
						if ((x0 + ix) < _nx)
						{
							_data[(y0 + iy), (x0 + ix)] = a._data[iy, ix];
						}
					}
				}
			}
		}

		public void Fill(T val)
		{
			for (int iy = 0; iy < _ny; iy++)
			{
				for (int ix = 0; ix < _nx; ix++)
				{
					_data[iy, ix] = val;
				}
			}
		}
	}
}
