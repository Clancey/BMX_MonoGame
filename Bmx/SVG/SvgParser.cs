//
//  Copyright 2012  James Clancey, Xamarin Inc  (http://www.xamarin.com)
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Xml;
using System.IO;
using System.Drawing;

namespace GameStateManagement
{
	public class SvgParser
	{
		string file;

		public SvgParser (string file)
		{
			this.file = file;
		}

		public void Parse ()
		{
			using (var reader = XmlReader.Create(File.Open(file, FileMode.Open))) {
				while (reader.Read()) {
					switch (reader.NodeType) {
					case XmlNodeType.Element:
						processElement (reader);
						//Console.WriteLine(reader.LocalName);
						//Console.WriteLine(reader.Value);

						break;

					}

				}
			}
		}

		void processElement (XmlReader reader)
		{
			string path = "";
			string label = "";
			PointF transform;
			if (!reader.LocalName.Contains ("path") || !reader.HasAttributes)
				return;
			while (reader.MoveToNextAttribute()) {
				switch (reader.LocalName) {
				case "d":
					path = reader.Value;
					break;
				case "label":

					break;
				case "transform":
					break;
				}
				Console.WriteLine (reader.LocalName);
				Console.WriteLine (reader.Value);
			}
		}

		void proccessTransform()
		{

		}

	}
}

