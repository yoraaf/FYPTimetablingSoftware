using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FYPTimetablingSoftware {
    class XMLParser {
        private string FileStr;
        private XmlDocument doc = new XmlDocument();
        private readonly XmlNode root;
        private Room[] RoomList;

        public XMLParser(string fileStr) {
            FileStr = fileStr;
            Console.WriteLine(fileStr);
            try { doc.Load(FileStr); } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            root = doc["timetable"];
            ReadAllRooms();

            XmlNode classes = root["classes"];
            XmlNode class1 = classes.ChildNodes.Item(0);
            Console.WriteLine("class1> "+class1.OuterXml);
            //XmlNode[] nodeList = new XmlNode[10];
            Klas[] klasList = new Klas[10];
            for (int i = 0; i < 10; i++) {
                XmlNode node = classes.ChildNodes.Item(i);
                //Room[] roomList = new Room[node.ChildNodes];
                XmlAttributeCollection attr = node.Attributes;
                string test = node.Attributes.GetNamedItem("id").Value;
                klasList[i] = new Klas(GetIntAttr(node, "id"), GetIntAttr(node, "offering"), GetIntAttr(node, "config"), GetIntAttr(node, "subpart"), GetIntAttr(node, "classLimit"), GetIntAttr(node, "department") );
            }
            Console.WriteLine("test");
        }

        private int GetIntAttr(XmlNode node, string name) {
            try {
                return Int32.Parse(node.Attributes.GetNamedItem(name).Value);
            } catch {
                return -1;
            }
        }

        private string GetStringAttr(XmlNode node, string name) {
            return node.Attributes.GetNamedItem(name).Value;
        }

        private void ReadAllRooms() {
            XmlNode RoomNodes = root["rooms"];
            RoomList = new Room[RoomNodes.ChildNodes.Count]; //set size for the room array

            for(int i = 0; i<RoomNodes.ChildNodes.Count; i++) {
                XmlNode currentRoom = RoomNodes.ChildNodes.Item(i);
                XmlAttributeCollection at = currentRoom.Attributes; //this will contain id, constraint, cap, loc
                int id = Int32.Parse(at.GetNamedItem("id").Value);
                int cap = Int32.Parse(at.GetNamedItem("capacity").Value);
                string[] locationString = at.GetNamedItem("location").Value.Split(',');
                int[] loc = new int[] { Int32.Parse(locationString[0]), Int32.Parse(locationString[1]) };
                if (currentRoom.HasChildNodes) {
                    XmlNode sharingNode = currentRoom.FirstChild;
                    string pattern = sharingNode.ChildNodes.Item(0).InnerText;
                    string ffa = sharingNode.ChildNodes.Item(1).Attributes.GetNamedItem("value").Value;
                    string na = sharingNode.ChildNodes.Item(2).Attributes.GetNamedItem("value").Value;
                    int nrOfDepartments = sharingNode.ChildNodes.Count - 3;
                    int[,] departments = new int[nrOfDepartments, 2];

                    for (int j = 3; j < sharingNode.ChildNodes.Count; j++) {
                        if(sharingNode.ChildNodes.Item(j).Name == "department") {
                            departments[j - 3, 0] = Int32.Parse(sharingNode.ChildNodes.Item(j).Attributes.GetNamedItem("value").Value);
                            departments[j - 3, 1] = Int32.Parse(sharingNode.ChildNodes.Item(j).Attributes.GetNamedItem("id").Value);
                        } else {
                            Console.WriteLine("Something went wrong reading the departments");
                        }
                    }

                    RoomSharing sharing = new RoomSharing(pattern, ffa, na, departments);
                    RoomList[i] = new Room(id, true, cap, loc, sharing);
                } else {
                    RoomList[i] = new Room(id, true, cap, loc);
                }
                
            }

            Console.WriteLine("parsed all rooms"+RoomList);
        }
    }
}
