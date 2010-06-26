﻿//-----------------------------------------------------------------------
// <copyright file="DeviceRequest.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.OAuth2.Messages {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Text;
	using DotNetOpenAuth.Messaging;

	/// <summary>
	/// A request from a rich app Client to an Authorization Server requested 
	/// authorization to access user Protected Data.
	/// </summary>
	internal class DeviceRequest : MessageBase, IOAuthDirectResponseFormat {
		/// <summary>
		/// A constant that identifies the type of message coming into the auth server.
		/// </summary>
		[MessagePart(Protocol.type, IsRequired = true)]
		private const string MessageType = "device_code";

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceRequest"/> class.
		/// </summary>
		/// <param name="tokenEndpoint">The authorization server.</param>
		/// <param name="version">The version.</param>
		internal DeviceRequest(Uri tokenEndpoint, Version version)
			: base(version, MessageTransport.Direct, tokenEndpoint) {
			this.HttpMethods = HttpDeliveryMethods.GetRequest;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceRequest"/> class.
		/// </summary>
		/// <param name="authorizationServer">The authorization server.</param>
		internal DeviceRequest(AuthorizationServerDescription authorizationServer)
			: this(authorizationServer.TokenEndpoint, authorizationServer.Version) {
			Contract.Requires<ArgumentNullException>(authorizationServer != null);
			Contract.Requires<ArgumentException>(authorizationServer.Version != null);
			Contract.Requires<ArgumentException>(authorizationServer.TokenEndpoint != null);

			// We prefer URL encoding of the data.
			this.Format = ResponseFormat.Form;
		}

		/// <summary>
		/// Gets the format the client is requesting the authorization server should deliver the request in.
		/// </summary>
		/// <value>The format.</value>
		ResponseFormat IOAuthDirectResponseFormat.Format {
			get { return this.Format.HasValue ? this.Format.Value : ResponseFormat.Json; }
		}

		/// <summary>
		/// Gets or sets the client identifier previously obtained from the Authorization Server.
		/// </summary>
		/// <value>The client identifier.</value>
		[MessagePart(Protocol.client_id, IsRequired = true, AllowEmpty = false)]
		internal string ClientIdentifier { get; set; }

		/// <summary>
		/// Gets or sets the scope.
		/// </summary>
		/// <value>The Authorization Server MAY define authorization scope values for the Client to include.</value>
		[MessagePart(Protocol.scope, IsRequired = false, AllowEmpty = true)]
		internal string Scope { get; set; }

		/// <summary>
		/// Gets or sets the format the client is requesting the authorization server should deliver the request in.
		/// </summary>
		/// <value>The format.</value>
		[MessagePart(Protocol.format, Encoder = typeof(ResponseFormatEncoder))]
		private ResponseFormat? Format { get; set; }
	}
}