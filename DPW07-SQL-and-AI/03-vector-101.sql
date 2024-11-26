declare 
    @dog vector(1536), 
    @cat vector(1536),
    @wolf vector(1536);

exec get_embedding 'dog', @dog output;
exec get_embedding 'cat', @cat output;
exec get_embedding 'wolf', @wolf output;

select
    *
from    
    ( values
        ('dog', 'cat', vector_distance('cosine', @dog, @cat)),
        ('dog', 'wolf', vector_distance('cosine', @dog, @wolf)),
        ('cat', 'wolf', vector_distance('cosine', @cat, @wolf))    
    ) as t (animal1, animal2, distance)
order by
    distance

