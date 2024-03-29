﻿
using System.Text.Json;

namespace bislerium.Data
{
    public class MemberService
    {
        public static List<Members> GetAll()
        {
            string membersFilePath = Utils.GetMembersFilePath();
            if(!File.Exists(membersFilePath))
            {
                return new List<Members>();
            }

            var json = File.ReadAllText(membersFilePath);

            if (json.Trim().Length > 0)
            {
                var deserializedData = JsonSerializer.Deserialize<List<Members>>(json);
                if (deserializedData != null)
                {
                    return deserializedData;
                }
            }

            return new List<Members>();
        }

        //To save all the members
        public static void SaveAll(List<Members> members)
        {
            string membersFilePath = Utils.GetMembersFilePath();

            var json = JsonSerializer.Serialize(members);
            File.WriteAllText(membersFilePath, json);
        }

        public static List<Members> Create(Guid userId, string memberName, string memberNumber)
        {
            List<Members> members = GetAll();
            bool memberExists = members.Any(x => x.MemberNumber == memberNumber);
            
            try
            {
                long numberValidation = long.Parse(memberNumber);
                if(numberValidation < 9700000000)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception("Please enter a valid number.");
            }

            if (memberExists)
            {
                throw new Exception("Member already exists.");
            }

            members.Add(
                new Members
                {
                    MemberNumber = memberNumber,
                    MemberName = memberName,
                    CreatedBy = userId
                }
            );

            SaveAll(members);
            return members;
        }

        public static Members GetByNumber(string memberNumber)
        {
            List<Members> members = GetAll();
            return members.FirstOrDefault(x => x.MemberNumber == memberNumber);
        }

        public static MemberType CheckMemberType(string memberNumber)
        {
            Members member = GetByNumber(memberNumber);
            if(member.MemberType == MemberType.Regular)
            {
                return MemberType.Regular;
            }
            
            return MemberType.General;
        }

        public static void ConvertMemberType(string memberNumber)
        {
            List<Members> members = GetAll();
            var memberToConvert = members.FirstOrDefault(x => x.MemberNumber == memberNumber);
            memberToConvert.MemberType = MemberType.Regular;
            int index = members.FindIndex(x => x.MemberNumber == memberNumber);

            if(index != -1)
            {
                members[index] = memberToConvert;
                SaveAll(members);
            }
            else
            {
                throw new Exception("Member not found.");
            }
        }
    }
}
