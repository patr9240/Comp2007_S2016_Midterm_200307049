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
                Session["SortColumn"] = "TodoID"; //default sort column
                Session["SortDirection"] = "Ascending"; // default sort direction
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
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                //query the Todos table using EF and LINQ
                var Todos = (from allTodos in db.Todos
                             select allTodos);

                //bind results to gridview
                TodoGridView.DataSource = Todos.AsQueryable().OrderBy(SortString).ToList();
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
         * This event handler allows pagination for the TodoList page
         * </summary>
         * @method TodosGridView_PageIndexChanging
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @returns {void}
         * */
        protected void TodosGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Set the new page number
            TodoGridView.PageIndex = e.NewPageIndex;

            //refresh the grid
            this.GetTodos();
        }
        /**
         * <summary>
         * This method changes the amount of todos displayed per page when a different index is selected in the dropdown
         * </summary>
         * @method PageSizeDropDownList_SelectedIndexChanged
         * @param {object} sender
         * @param {EventArgs} e
         * @returns {void}
         * */
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the new page size
            TodoGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            //refresh
            this.GetTodos();
        }

        /**
         * <summary>
         * This method sorts the todos list gridview
         * </summary>
         * @method TodoGridView_Sorting
         * @returns {void}
         * */
        protected void TodoGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            
            //refresh the grid
            this.GetTodos();

            //toggle the direction from ASC and DSC
            Session["SortDirection"] = Session["SortDirection"].ToString() == "Ascending" ? "Descending" : "Ascending";


        }

        /**
         * <summary>
         * This method sorts the todos list gridview
         * </summary>
         * @method TodoGridView_RowDataBound
         * @returns {void}
         * */
        protected void TodoGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)//if header row is clicked
                {
                    LinkButton linkButton = new LinkButton();

                    for (int index = 0; index < TodoGridView.Columns.Count - 1; index++)
                    {
                        if (TodoGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "Ascending")
                            {
                                linkButton.Text = "<i class='fa fa-caret-up fa-lg'> </i>";
                            }
                            else
                            {
                                linkButton.Text = "<i class='fa fa-caret-down fa-lg'> </i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkButton);
                        }
                    }
                }
            }
        }
    }
}