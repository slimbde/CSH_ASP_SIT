--select u.Surname, u.Name, u.Patronic, v.VacationRating, v.Voted, u.Id
--from Votings as v
--inner join AspNetUsers as u
--on u.Id=v.UsrId
--and v.Voted=1
--order by u.Surname

--select u.Surname, u.Name, v.Year, v.Duration
--from Vacations as v
--inner join AspNetUsers as u
--on v.UsrId=u.Id
--where v.Year=2020
--order by u.Surname

