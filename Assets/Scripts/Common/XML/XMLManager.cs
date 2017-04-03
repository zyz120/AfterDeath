using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using LitJson;
using System.Collections.Generic;

namespace XML
{
	public class XMLManager{
		
		private static XMLManager _instance;

		string path = Application.dataPath + "/player.xml";

		public static XMLManager GetInstance(){
			if (_instance == null) {
				_instance = new XMLManager ();
			}
			return _instance;
		}

		public bool CheckXML(){
			return File.Exists (path);
		}

		public void Create(){
			//create a xml document instance
			XmlDocument xmlDoc = new XmlDocument ();
			//create a root node
			XmlElement root = xmlDoc.CreateElement ("player");

			//create the modules of the xml
			CreateSkillModule (xmlDoc, root);
			CreateResourceModule (xmlDoc, root);
			CreateCheckPointModule (xmlDoc, root);

			//append the root node to the XmlDocument
			xmlDoc.AppendChild(root);

			//save the XmlDocument
			xmlDoc.Save(path);
			Debug.Log ("Create a new save");
		}

		public Player LoadPlayerInfo(){
			Player player = new Player();

			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.Load (path);

			LoadResource (xmlDoc, player);
			LoadSkill (xmlDoc, player);
			LoadCheckPoint (xmlDoc, player);

			return player;
		}

		public void LoadPlayerInfo(Player player)
		{
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.Load (path);

			LoadResource (xmlDoc, player);
			LoadSkill (xmlDoc, player);
			LoadCheckPoint(xmlDoc, player);
		}

		public void UpdatePlayerInfo(Player player)
		{
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.Load (path);

			UpdateResource (xmlDoc, player);

			xmlDoc.Save (path);
			Debug.Log ("Update the xml document");
		}
			

		void CreateSkillModule(XmlDocument xmlDoc,XmlElement root)
		{
			XmlElement skillNode = xmlDoc.CreateElement ("skill");

			root.AppendChild (skillNode);
		}

		void CreateResourceModule(XmlDocument xmlDoc,XmlElement root)
		{
			//create node of next level
			XmlElement eleRes = xmlDoc.CreateElement ("resource");
			eleRes.SetAttribute("id","0");

			//create specific resources node
			XmlElement eleJinshucanhai = xmlDoc.CreateElement("JinShuCanHai");
			eleJinshucanhai.SetAttribute ("id", "1");
			eleJinshucanhai.InnerText = "0";
			XmlElement eleWasi = xmlDoc.CreateElement ("WaSi");
			eleWasi.SetAttribute ("id", "2");
			eleWasi.InnerText = "0";
			XmlElement eleShiyou = xmlDoc.CreateElement ("ShiYou");
			eleShiyou.SetAttribute ("id", "3");
			eleShiyou.InnerText = "0";
			XmlElement eleKuangshi = xmlDoc.CreateElement ("KuangShi");
			eleKuangshi.SetAttribute ("id", "4");
			eleKuangshi.InnerText = "0";
			XmlElement eleLieyanshi = xmlDoc.CreateElement ("LieYanShi");
			eleLieyanshi.SetAttribute ("id", "5");
			eleLieyanshi.InnerText = "0";
			XmlElement eleMingjiehua = xmlDoc.CreateElement ("MingJieHua");
			eleMingjiehua.SetAttribute ("id", "6");
			eleMingjiehua.InnerText = "0";
			XmlElement eleHuxiaoshi = xmlDoc.CreateElement ("HuXiaoShi");
			eleHuxiaoshi.SetAttribute ("id", "7");
			eleHuxiaoshi.InnerText = "0";

			//append specific resource node to the total resource node
			eleRes.AppendChild(eleJinshucanhai);
			eleRes.AppendChild (eleWasi);
			eleRes.AppendChild (eleShiyou);
			eleRes.AppendChild (eleKuangshi);
			eleRes.AppendChild (eleLieyanshi);
			eleRes.AppendChild (eleMingjiehua);
			eleRes.AppendChild (eleHuxiaoshi);

			//append all class node to the root node
			root.AppendChild(eleRes);
		}

		void CreateCheckPointModule(XmlDocument xmlDoc,XmlElement root)
		{
			XmlElement skillNode = xmlDoc.CreateElement ("skill");

			root.AppendChild (skillNode);
		}

		void LoadSkill(XmlDocument xmlDoc, Player player){
		}

		void LoadCheckPoint(XmlDocument xmlDoc,Player player){
		}

		void LoadResource(XmlDocument xmlDoc,Player player){
			XmlElement resourceNode = GetModule (xmlDoc, "resource");

			foreach (XmlElement xe in resourceNode) {
				player.SetResourceNum (xe.Name, int.Parse (xe.InnerText));
			}
		}

		void UpdateResource(XmlDocument xmlDoc, Player player)
		{
			Dictionary<string,int> resources = player.GetResourceNum ();

			XmlElement resourceNode = GetModule (xmlDoc, "resource");

			foreach (XmlElement xe in resourceNode) {
				xe.InnerText = resources[xe.Name].ToString ();
			}
		}

		XmlElement GetModule(XmlDocument xmlDoc, string moduleName)
		{
			XmlNodeList nodeList = xmlDoc.SelectSingleNode ("player").ChildNodes;

			foreach (XmlElement xe in nodeList) {
				if (xe.Name == moduleName) {
					return xe;
				}
			}
			return null;
		}
	}
}