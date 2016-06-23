using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using COMP2007_S2016_MidTerm_200307049.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

/**
 * @author: Patrick Ross
 * @date: June 23rd, 2016
 * @version: 0.0.1 - Created page
 */
namespace COMP2007_S2016_MidTerm_200307049
{
    public partial class TodoList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading page for the first time, populate the grid
            if (!IsPostBack)
            {
                //get the todo list data
                this.GetTodos();
            }
        }

        /**
         * <summary>
         * This method gets the todos from the database and puts them into the gridview
         * </summary>
         * @method GetTodos
         * @return {void}
         * */
        protected void GetTodos()
        {

            //connect to EF
            using (TodoConnection db = new TodoConnection())
            {
                //query the Games table using EF and LINQ
                var Todos = (from allTodos in db.Todos
                             select allTodos);

                //bind results to gridview
                TodoGridView.DataSource = Todos.AsQueryable().ToList();
                TodoGridView.DataBind();
            }
        }

        /**
         * <summary>
         * This event handler deletes a todo from the databse using EF
         * </summary>
         * @method TodoGridView_RowDeleting
         * @param {object} sender
         * @param {GridViewDeleteEventArgs}
         * @returns {void}
         * */
        protected void TodoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store which row was clicked
            int selectedRow = e.RowIndex;

            //get the selected todoid using the grids datakey collection
            int TodoID = Convert.ToInt32(TodoGridView.DataKeys[selectedRow].Values["TodoID"]);

            //use ef to find the selected todo and delete it
            using (TodoConnection db = new TodoConnection())
            {
                //create object of the todo class and store the query string inside of it
                Todo deletedTodo = (from todoRecords in db.Todos
                                    where todoRecords.TodoID == TodoID
                                    select todoRecords).FirstOrDefault();

                //remove the selected todo from the db
                db.Todos.Remove(deletedTodo);

                //save db changes
                db.SaveChanges();

                //refresh gridview
                this.GetTodos();

            }
        }

        /**
         * <summary>
         * This method changes the number of todos per page
         * </summary>
         * @method TrackingWeekDropDown_SelectedIndexChanged
         * @return {void}
         * */
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.GetGames();
        }
    }
}