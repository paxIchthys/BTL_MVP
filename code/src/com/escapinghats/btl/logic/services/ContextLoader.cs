using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

using com.escapinghats.btl.config;
using com.escapinghats.btl.logic.schemas.context;

namespace com.escapinghats.btl.logic.services{

	public partial class ContextLoader
	{
		public const string CONTEXT_FILE_LOCATION = Config.GAME_DATA; //There should only be one file link in code. All others should be in this file
		public const string CONTEXT_FILE_NAME = "btl.json";
		//public const string CONTEXT_FILE_LOCATION = "https://btl-builder.herokuapp.com/btl.json"; //There should only be one file link in code. All others should be in this file
		
		public ContextDefinition ContextDef;
		public event EventHandler<LoaderEventArgs> LoaderEvent;
		private HttpRequest httpRequest;
		public enum LoadTypes { Context, Data };
		public LoadTypes loadType;

		private string currentFileLocation;
		
		public void Initialize(HttpRequest httpRequest){
			Debug.Instance.Log("ContextLoader initializing");
			this.httpRequest = httpRequest;
			httpRequest.RequestCompleted += OnHttpRequestCompleted;
			httpRequest.Timeout = 10;

			loadType = LoadTypes.Context;
			LoadFile(CONTEXT_FILE_LOCATION, LoadTypes.Context, CONTEXT_FILE_NAME);
		}

		public void LoadFile(string fileLocation, LoadTypes loadType, string fileName){
			this.loadType = loadType;
			currentFileLocation = fileName;

			if(fileLocation == null || fileLocation == ""){
				Debug.Instance.Error("File location is null or empty");
				return;
			}

			if(httpRequest == null){
				Debug.Instance.Error("HttpRequest is null");
				return;
			}

			if(fileLocation.Substring(0,3) == "res"){
				Godot.FileAccess file = Godot.FileAccess.Open(fileLocation, Godot.FileAccess.ModeFlags.Read);
				string json = "";
				if(file != null){
					json = file.GetAsText();
					Debug.Instance.Log("JSON loaded successfully from res: " + json);
				}
				file.Close();
				HandleFileLoaded(json);
			}else{
				Debug.Instance.Log("Loading JSON from URL: " + fileLocation + " " + loadType);
				try{
					Error error = httpRequest.Request(fileLocation);	
					if(error != Error.Ok){
						Debug.Instance.Error("Error loading JSON from URL: " + error);
						LoaderEvent?.Invoke(this,new LoaderEventArgs(LoaderEventArgs.LoaderEventTypes.Error, fileLocation, "", error.ToString()));			
					}
				} catch (Exception ex){
					Debug.Instance.Error("Error loading JSON from URL: " + ex.Message);
					LoaderEvent?.Invoke(this,new LoaderEventArgs(LoaderEventArgs.LoaderEventTypes.Error, fileLocation, "", ex.Message));			
				}			
			}			
	
	}

		private void HandleFileLoaded(string fileContent){
			if(loadType == LoadTypes.Context){
				SetContext(fileContent);
			}else if(loadType == LoadTypes.Data){
				Debug.Instance.Log("HandleFileLoaded::DATA_LOADED:");
				LoaderEvent?.Invoke(this,new LoaderEventArgs(LoaderEventArgs.LoaderEventTypes.DataLoaded, currentFileLocation, fileContent, "Data loaded successfully"));				
			}

		}

		private void SetContext(string json){
			ContextDef = JsonSerializer.Deserialize<ContextDefinition>(json);
			httpRequest.Timeout = 0;
			LoaderEvent?.Invoke(this,new LoaderEventArgs(LoaderEventArgs.LoaderEventTypes.ContextLoaded, currentFileLocation, json, "Context loaded successfully"));			
		}

		private void OnHttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
		{
			Debug.Instance.Log("HTTP Request completed" + result + " " + responseCode);
			if (result != 0)
			{
				Debug.Instance.Error($"Error downloading JSON from URL result: {Enum.GetName(typeof(Error), result)} " + responseCode);
				if(currentFileLocation == CONTEXT_FILE_NAME){
					Debug.Instance.Error("Context file not found");
					LoaderEvent?.Invoke(this,new LoaderEventArgs(LoaderEventArgs.LoaderEventTypes.Error, currentFileLocation, "", "Context file not found"));					
				}
				return;
			}

			Debug.Instance.Log("HTTP Request completed" + result + " " + responseCode);
			if (responseCode != 200)
			{
				Debug.Instance.Error($"Error downloading JSON from URL code: {responseCode}");
				return;
			}

			string json = System.Text.Encoding.UTF8.GetString(body);
			Debug.Instance.Log("JSON loaded successfully: " + json);
			HandleFileLoaded(json);
		}

	}

	public class LoaderEventArgs : EventArgs
	{
		public LoaderEventTypes Type;
		public string FileName;
		public string FileContents;
		public string Message;
		public enum LoaderEventTypes { ContextLoaded, DataLoaded, Error };

		public LoaderEventArgs(LoaderEventTypes type, string fileName, string fileContents, string message)
		{
			Type = type;
			FileName = fileName;
			FileContents = fileContents;
			Message = message;
		}
	}
}
