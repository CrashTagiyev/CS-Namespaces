using PostSpace;
using AdminSpace;
using UserSpace;
using StepSpace;
using PersonSpace;
using System;
using System.Net.Cache;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

static class UsefullMethods
{
    static public int UserID { get; set; } = 1;
    static public int AdminID { get; set; } = 1;
    static public int PostID { get; set; } = 1;
    static public string NewGuid()
    {
        string newId = "";
        for (int i = 0; i < 8; i++)
        {
            newId += Guid.NewGuid().ToString()[i];
        }
        return newId;
    }
}
static class Notifications
{
    static public string? LikeNot { get; set; }
    static public string? WatchNotification { get; set; }
    static public string? CommentNotification { get; set; }
    static public string GetLikeNot(User username)
    {
        return $"{DateTime.Now}\n{username.Username} liked your post";
    }
    static public string GetWatchNot(User username)
    {
        return $"{DateTime.Now}\n{username.Username} watched your post";
    }
    static public string GetCommentNot(User username)
    {
        return $"{DateTime.Now}\n{username.Username}  comment your post";
    }
}


namespace PostSpace
{
    public class Post
    {
        public Post(string? content)
        {
            Id = UsefullMethods.PostID++;
            CreationDateTime = DateTime.Now.ToString();
            Content = content;
            UsersWhoWatched = new List<Person> { };
            UsersWhoLiked = new List<Person> { };
            Comments = new List<string> { };
        }
        //properties
        public int? Id { get; set; }
        private string? Content { get; set; }
        private string? CreationDateTime { get; set; }
        public int ViewCount { get; set; }
        private int LikeCount { get; set; }
        private int CommentCount { get; set; }
        private List<Person> UsersWhoWatched { get; set; }
        private List<Person> UsersWhoLiked { get; set; }
        private List<string> Comments { get; set; }
        //methods
        public override string ToString()
        {
            return $"ID:{Id}\nCreated:{CreationDateTime}\nWatched:{ViewCount}\nLikes:{LikeCount}\n\n{Content}\n";
        }
        public void ShowComments()
        {
            foreach (var item in Comments)
            {
                Console.WriteLine(item);
            }
        }
        public void ShowContent(Person username)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine(this);
            Console.WriteLine($"Comments:{CommentCount}");
            ShowComments();
            Console.WriteLine("---------------------------------");
            ++ViewCount;
            UsersWhoWatched.Add(username);
            Thread.Sleep(4000);
        }
        public void ShowContent(Admin adminname)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine(this);
            Console.WriteLine($"Comments:{CommentCount}");
            ShowComments();
            Console.WriteLine("---------------------------------");

        }
        public void LikeTheContent(Person username)
        {
            ++LikeCount;
            UsersWhoLiked.Add(username);
        }

        public void AddComment(string newcomment)
        {
            Comments.Add(newcomment);
            ++CommentCount;
        }

        public void ShowWhoWatchedThisPost()
        {
            foreach (var item in UsersWhoWatched)
            {
                Console.WriteLine(item);
            }
        }
        public void ShowWhoLikedThisPost()
        {
            foreach (var item in UsersWhoLiked)
            {
                Console.WriteLine(item);
            }
        }
    }
}

namespace PersonSpace
{
    public class Person
    {
        public Person(string? username, string? password)
        {
            Id = UsefullMethods.AdminID;
            Username = username;
            Password = password;
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

namespace AdminSpace
{
    public class Admin : Person
    {
        //Constructor
        public Admin(string? username, string? password) : base(username, password)
        {


            Posts = new List<Post> { };
            Notifications = new List<string?> { };
        }
        //properties
        public List<Post> Posts { get; set; }
        public List<string?> Notifications { get; set; }

        public override string ToString()
        {
            return $"{Username}";
        }
        //methods
        public void MakeAPost(string? content)
        {
            AddToPosts(new Post(content));
        }
        private void AddToPosts(Post postname)
        {
            Posts.Add(postname);
        }
        public void ShowNotifications()
        {
            foreach (var item in Notifications)
            {
                Console.WriteLine(item);
            }
            Thread.Sleep(4000);
        }
        public void AddNotification(string newNot)
        {
            Notifications.Add(newNot);
        }
        public void ShowAllThePosts(Person username)
        {
            for (int i = 0; i < Posts.Count; i++)
            {
                Posts[i].ShowContent(username);
            }
        }
    }
}

namespace UserSpace
{
    public class User : Person
    {
        public User(string? username, DateTime BirthTime, string? email, string? password) : base(username, password)
        {
            Age = DateTime.Now.Year - BirthTime.Year;
            Email = email;
            Password = password;
        }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public void CommentOnPost(Post postname, string comment)
        {
            postname.AddComment(comment);
        }

        public override string ToString()
        {
            return $"ID:{Id}:{Username}";
        }
    }
}

namespace StepSpace
{
    public class StepBook
    {
        public StepBook(Admin admin, List<User> users)
        {
            Admin = admin;
            Users = users;
        }
        public Admin Admin { get; set; }
        public List<User> Users { get; set; }
        //methods
        private bool AdminPassCheck(string? password)
        {
            if (password == Admin.Password) return true;
            return false;
        }
        private bool UserPassCheck(string? password)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Password == password) return true;
            }
            return false;
        }
        private int? FindUser(string? password)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Password == password) return i;
            }
            return null;
        }
        public void PassControl(int choice)
        {
            Console.Clear();
            try
            {

                if (choice != 1 && choice != 2)
                {
                    throw new Exception("You can choice only 1 or 2 numbers");
                }
                if (choice == 1)
                {
                    Console.WriteLine("Write the admins password");
                    string? AdminPass = Console.ReadLine();
                    if (!AdminPassCheck(AdminPass)) throw new Exception("wrong pass try again");
                    else
                    {
                        Console.WriteLine($"Hello {Admin.Username}");
                        AdminInterface(Admin);
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Write user passwoord");
                    string? UserPass = Console.ReadLine();
                    if (!UserPassCheck(UserPass)) throw new Exception("wrong pass try again");
                    else
                    {
                        Console.WriteLine($"Welcome {Users[Convert.ToInt32(FindUser(UserPass))].Username}");
                        int? userindex = FindUser(UserPass);
                        UserInterface(Users[Convert.ToInt32(userindex)]);
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                PassControl(choice);
            }
        }
        public void ShowPosts()
        {

            foreach (var item in Admin.Posts)
            {
                Console.WriteLine(item);
            }
            Thread.Sleep(4000);
        }
        private int FindPostByID(int ID)
        {
            for (int i = 0; i < Admin.Posts.Count; i++)
            {
                if (ID == Admin.Posts[i].Id)
                {
                    return i;
                }
            }
            return -1;
        }
        private void ShowPostsForAdmin()
        {
            Console.Clear();
            for (int i = 0; i < Admin.Posts.Count; i++)
            {
                Admin.Posts[i].ShowContent(Admin);
            }
            Thread.Sleep(5000);
        }
        private void AdminInterface(Admin admin)
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Choice.\n1:Create new post\n2:Watch Notifications\n3:Show posts\n4:Exit");
                int choice = int.Parse(Console.ReadLine());
                if (choice != 1 && choice != 2 && choice != 3 && choice != 4) throw new Exception("You can only chocie 1 or 2");
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Write a new post");
                        string? newContent = Console.ReadLine();
                        Admin.MakeAPost(newContent); break;
                    case 2: Admin.ShowNotifications(); break;
                    case 3: ShowPostsForAdmin(); break;
                    case 4: return;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            AdminInterface(admin);
        }
        private void UserInterface(User username)
        {
            Console.Clear();
            try
            {
                Console.WriteLine("Choice.\n1:Show all posts\n2:Show post by ID\n3:Like the post by ID\n4:Comment the post\n5:Exit");
                int userchoice = int.Parse(Console.ReadLine());

                if (userchoice != 1 && userchoice != 2 && userchoice != 3 && userchoice != 4 && userchoice != 5) throw new Exception("You can only choice 1,2,3 or 4");
                switch (userchoice)
                {
                    case 1: Admin.ShowAllThePosts(username); 
                        Admin.AddNotification($"{DateTime.Now}\n{username.Username} watched all your posts");
                        break;
                    case 2:
                        Console.WriteLine("Write iD of the post you want watch");
                        int userShowPostIndexBy = int.Parse(Console.ReadLine()) - 1;
                        Admin.Posts[userShowPostIndexBy].ShowContent(username);
                        Admin.AddNotification($"{DateTime.Now}\n{username.Username} watched your post");
                        Thread.Sleep(4000); break;
                    case 3:
                        Console.WriteLine("Write iD of the post you want like");
                        int userLikePostIndexBy = int.Parse(Console.ReadLine()) - 1;
                        Admin.AddNotification($"{DateTime.Now}\n{username.Username} Liked your post");
                        Admin.Posts[userLikePostIndexBy].LikeTheContent(username); break;
                    case 4:
                        Console.WriteLine("Write iD of the post you want comment");
                        int userCommentPostIndexBy = int.Parse(Console.ReadLine()) - 1;
                        Console.WriteLine($"\n{userCommentPostIndexBy}");
                        int userCommentPostIndexBy2 = FindPostByID(userCommentPostIndexBy);
                        Console.WriteLine("Write a new comment");
                        Admin.AddNotification($"{DateTime.Now}\n{username.Username} commented to your post");
                        string newComment = Console.ReadLine();
                        Admin.Posts[userCommentPostIndexBy].AddComment($"{username.Username}:{newComment}");
                        break;
                    case 5: return;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            UserInterface(username);
        }

        public void start()
        {
            Console.Clear();
            try
            {

                Console.WriteLine("Choice.\n1:Admin\n2:User\n");
                int Choice = int.Parse(Console.ReadLine());
                if (Choice != 1 && Choice != 2)
                {
                    throw new Exception("You can input only 1 or 2 numbers");
                }
                PassControl(Choice);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                start();
            }
            start();
        }
    }
}
internal class Program
{

    static void Main(string[] args)
    {
        User u1 = new User("Emil", new DateTime(1994, 6, 17), "qweqwe@mail.com", "1111");
        User u2 = new User("Cmil", new DateTime(1994, 6, 17), "qweqwe@mail.com", "1212");
        User u3 = new User("Tamil", new DateTime(1994, 6, 17), "qweqwe@mail.com", "1313");
        List<User> users = new List<User> { u1, u2, u3 };
        Admin admin = new Admin("Crash", "1414");
        StepBook stepBook = new StepBook(admin, users);

        stepBook.start();






    }
}