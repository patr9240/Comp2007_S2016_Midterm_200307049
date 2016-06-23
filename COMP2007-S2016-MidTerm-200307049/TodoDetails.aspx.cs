using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using COMP2007_S2016_MidTerm_200307049.Models;
using System.Web.ModelBinding;

/**
 * @author: Patrick Ross
 * @date: June 23rd, 2016
 * @version: 0.0.1 - Page Created
 */
namespace COMP2007_S2016_MidTerm_200307049
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }

        /**
        * <summary>
        * This method gets the todo being edited and enters it's data into the text fields
        * </summary>
        * @method GetTodo
        * @return {void}
        * */
        protected void GetTodo()
        {
            // populate the form with existing data from the database
            int TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

            // connect to the EF DB
            using (TodoConnection db = new TodoConnection())
            {
                // populate a todo object instance with the TodoID from the URL Parameter
                Todo UpdatedTodo = (from todo in db.Todos
                                    where todo.TodoID == TodoID
                                    select todo).FirstOrDefault();

                // map the todo properties to the form controls
                if (UpdatedTodo != null)
                {
                    TodoNameTextBox.Text = UpdatedTodo.TodoName;
                    TodoNotesTextBox.Text = UpdatedTodo.TodoNotes;
                    if (UpdatedTodo.Completed == true)
                    {
                        CompletedCheckBox.Checked = true;
                    }
                    else
                    {
                        CompletedCheckBox.Checked = false;
                    }
                }
            }
        }

        /**
         * <summary>
         * This method returns the user to TodoList page
         * </summary>
         * @method CancelButton_Click
         * @return {void}
         * */
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TodoList.aspx");
        }

        /**
         * <summary>
         * This method creates a todo object based on that information, then redirects the user back to the todolist page.
         * </summary>
         * @method SaveButton_Click
         * @return {void}
         * */
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (TodoConnection db = new TodoConnection())
            {
                // use the todo model to create a new todo object and save a new record
                Todo newTodo = new Todo();

                int TodoID = 0;

                if (Request.QueryString.Count > 0) // our URL has a TodoID in it
                {
                    // get the id from the URL
                    TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    // get the current todo from EF DB
                    newTodo = (from todo in db.Todos
                               where todo.TodoID == TodoID
                               select todo).FirstOrDefault();
                }

                // add form data to the new todo record
                newTodo.TodoName = TodoNameTextBox.Text;
                newTodo.TodoNotes = TodoNotesTextBox.Text;
                if (CompletedCheckBox.Checked == true)
                {
                    newTodo.Completed = true;
                }
                else
                {
                    newTodo.Completed = false;
                }
                
                // use LINQ to ADO.NET to add / insert new todo into the database
                if (TodoID == 0)
                {
                    db.Todos.Add(newTodo);
                }
                // save our changes - also updates and inserts
                db.SaveChanges();
                // Redirect back to the updated TodoList page
                Response.Redirect("~/TodoList.aspx");
            }
        }
    }
}