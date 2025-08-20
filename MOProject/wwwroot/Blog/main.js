fetch('Blog/blogData.json')
  .then(response => response.json())
  .then(posts => {
    const container = document.getElementById('blog-container');
    const fragment = document.createDocumentFragment();

    posts.forEach(post => {
      const div = document.createElement('div');
      div.classList.add('post-preview', 'mb-8');

      div.innerHTML = `
        <a href="/blog/${post.slug}">
          ${post.imageUrl ? `<img src="${post.imageUrl}" alt="${post.title}" class="w-full h-auto mb-4 rounded" />` : ''}
          <h2 class="post-title text-2xl font-bold">${post.title}</h2>
          <h3 class="post-subtitle text-lg text-gray-600">${post.subtitle}</h3>
        </a>
        <p class="post-meta text-sm text-gray-500 mt-2">
          Posted by <a href="#">${post.author}</a> on ${new Date(post.date).toLocaleDateString()}
        </p>
        <hr class="my-6 border-gray-300"/>
      `;

      fragment.appendChild(div);
    });

    container.appendChild(fragment);
  })
  .catch(error => {
    console.error('Error loading blog data:', error);
    document.getElementById('blog-container').innerHTML = "<p>Failed to load blog posts.</p>";
  });
