// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Web.Actors
{
	using System.Web;
	using System.Web.Routing;
	using Actions;
	using Binding;
	using Channels;

	/// <summary>
	/// ASP.NET Routing Handler for binding models
	/// </summary>
	/// <typeparam name="TInput"></typeparam>
	public class ActorRouteHandler<TInput> :
		IRouteHandler
	{
		private readonly ChannelProvider<TInput> _channelProvider;
		private readonly ModelBinder _modelBinder;
		private readonly ActionQueueProvider _queueProvider;

		public ActorRouteHandler(ActionQueueProvider queueProvider, ModelBinder modelBinder, ChannelProvider<TInput> channelProvider)
		{
			_modelBinder = modelBinder;
			_channelProvider = channelProvider;

			_queueProvider = queueProvider;
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var context = new HttpActorRequestContext(_queueProvider(), requestContext);

			var inputModel = (TInput) _modelBinder.Bind(typeof (TInput), context);

			var handler = new ActorHttpAsyncHandler<TInput>(context, inputModel, _channelProvider.GetChannel(inputModel));

			return handler;
		}
	}
}