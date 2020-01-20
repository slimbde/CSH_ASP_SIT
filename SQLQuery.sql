select u.Surname, u.Name, u.Patronic, v.VacationRating, v.Voted, u.Id, u.UserName
from Votings as v
inner join AspNetUsers as u
on u.Id=v.UsrId
and v.Voted=1
order by v.VacationRating

--select u.Surname, u.Name, v.Year, v.Duration
--from Vacations as v
--inner join AspNetUsers as u
--on v.UsrId=u.Id
--where v.Year=2020
--order by u.Surname

