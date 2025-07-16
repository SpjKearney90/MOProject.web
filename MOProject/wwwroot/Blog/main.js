fetch('Blog/blogData.json')
  .then(response => response.json())
  .then(posts => {
    const container = document.getElementById('blog-container');

    posts.forEach(post => {
      const postHTML = `
        <div class="post-preview">
          <a href="/blog/${post.slug}">
            <h2 class="post-title">${post.title}</h2>
            <h3 class="post-subtitle">${post.subtitle}</h3>
          </a>
          <p class="post-meta">
            Posted by <a href="#">${post.author}</a> on ${new Date(post.date).toLocaleDateString()}
          </p>
        </div>
        <hr class="my-4"/>
      `;

      container.insertAdjacentHTML('beforeend', postHTML);
    });
  })
  .catch(error => {
    console.error('Error loading blog data:', error);
    document.getElementById('blog-container').innerHTML = "<p>Failed to load blog posts.</p>";
  });
