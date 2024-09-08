
<script>
    const showcase = document.querySelector('.showcase')
    const menuToggle = document.querySelector('.toggle');
       menuToggle.addEventListener('click',()=>{
        showcase.classList.toggle('active')
        if(showcase.classList.contains('active')){
        menuToggle.innerHTML = '<i class="fas fa-times fa-2x"></i>'
    } else {
        menuToggle.innerHTML = '<i class="fas fa-bars fa-2x">'
    }
       })
</script>