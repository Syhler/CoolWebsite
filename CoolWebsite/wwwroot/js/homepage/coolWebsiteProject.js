$(document).ready(function () {

    const observerCoolWebsite = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#cool-website-project").addClass("active")
        }
    }, {threshold: [1]});

    const observerCoolWebsiteFeatures = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#cool-website-features").addClass("active")
        }
    }, {threshold: [1]});


    observerCoolWebsite.observe(document.querySelector(".cool-website-image"));
    observerCoolWebsiteFeatures.observe(document.querySelector(".cool-website-features"));


    function setAllNavLinkInactive() {

        $(".nav-link").each(function ()
        {
            $(this).removeClass("active")

        })

    }
})