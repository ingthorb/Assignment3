Changed CourseID which was text in Courses and CourseTemplate in TemplateID which there was CourseID which was also used in
StudentsInCourses which is int and is ambiguous.
Rearrenged Course database according to the tests.
Changed the CourseID in StudentsInCourses which was Text in the database but Int inside the project.

We implemented the IstheStudentinthecourse wrong, changed it.

We can add a course.

Added waitinglist

Changed StudentDTO we shouldn't be returning the id.

ADDstudentlist _id wasn't doing anything?
Do we need to change the Date format?
https://en.wikipedia.org/wiki/ISO_8601

Have a exception for everything, for example in AddTowaitinglist there are 2
exceptions returning the same status code. But it helps debugging later on.

Added students in Db how the db should be.

Added DeleteStudent
Added waitinglist, get and add.