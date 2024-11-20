declare 
    @dog vector(1536), 
    @cat vector(1536),
    @wolf vector(1536);

exec get_embedding 'dog', @dog output;
exec get_embedding 'cat', @cat output;
exec get_embedding 'wolf', @wolf output;

select
    vector_distance('cosine', @dog, @cat) as dog_vs_cat,
    vector_distance('cosine', @dog, @wolf) as dog_vs_wolf,
    vector_distance('cosine', @cat, @wolf) as cat_vs_wolf;
