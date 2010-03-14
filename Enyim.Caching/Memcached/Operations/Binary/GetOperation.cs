﻿
namespace Enyim.Caching.Memcached.Operations.Binary
{
	internal class GetOperation : ItemOperation
	{
		public GetOperation(IServerPool pool, string key) : base(pool, key) { }

		protected override bool ExecuteAction()
		{
			PooledSocket socket = this.Socket;
			if (socket == null) return false;

			BinaryRequest request = new BinaryRequest(OpCode.Get);
			request.Key = this.HashedKey;
			request.Write(socket);

			BinaryResponse response = new BinaryResponse();

			if (response.Read(socket))
			{
				int flags = BinaryConverter.DecodeInt32(response.Extra, 0);
				this.result = this.ServerPool.Transcoder.Deserialize(new CacheItem((ushort)flags, response.Data));

				return true;
			}

			return false;
		}

		private object result;

		public object Result
		{
			get { return this.result; }
		}

	}
}

#region [ License information          ]
/* ************************************************************
 *
 * Copyright (c) Attila Kiskó, enyim.com
 *
 * This source code is subject to terms and conditions of 
 * Microsoft Permissive License (Ms-PL).
 * 
 * A copy of the license can be found in the License.html
 * file at the root of this distribution. If you can not 
 * locate the License, please send an email to a@enyim.com
 * 
 * By using this source code in any fashion, you are 
 * agreeing to be bound by the terms of the Microsoft 
 * Permissive License.
 *
 * You must not remove this notice, or any other, from this
 * software.
 *
 * ************************************************************/
#endregion