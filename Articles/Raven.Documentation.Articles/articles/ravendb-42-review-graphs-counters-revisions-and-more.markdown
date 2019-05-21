# Graphs, Counters, Revisions and More: A RavenDB 4.2 Review<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![Graphs, Counters, Revisions and More: A RavenDB 4.2 Review](images/ravendb-42-review-graphs-counters-revisions-and-more.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="text-center">
<button id="podcast-play-button" class="play-button" style=""><i class="icon-play" style="margin-right:20px"></i>Play Podcast</button>
</p>



<br>

 <img src="images/oren.png" style="float: right; width: 17%; margin: 45px; margin-top: -10px;"><p style="text-align: justify">Just 14 months after we released a major version of RavenDB, we are pleased to announce the release of RavenDB 4.2. In this podcast, we go over all of the features that have graduated from experimental to fully operational, and the new features that will expand your ability to process data and keep up with the speed of information.</p>


<br>
#### RavenDB 4.2 Features include:
<ul>
    <li>Graph API</li>
    <li>Revert Revisions</li>
    <li>Pull Replication</li>
    <li>Clusterwide Transactions</li>
    <li>Distributed Counters</li>
    <li>JavaScript Indexes</li>
    </ul>
<br>
<audio id="podcast-audio" controls="" style="width: 100%">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/2019-04-29-podcast-ravendb-42.ogg" type="audio/ogg">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/2019-04-29-podcast-ravendb-42.mp3" type="audio/mpeg">
  Your browser does not support the audio element.
</audio>

{RAW}
<script>

function changeButtonToPlay(button) {
  button.className = "play-button";
  button.innerHTML = "<i class=\"icon-play\" style=\"margin-right:20px\"></i> Play Podcast"
}

function changeButtonToPause(button) {
  button.className = "play-button is-playing";
  button.innerHTML = "<i class=\"icon-pause\" style=\"margin-right:20px\"></i> Pause";
}

var audioElement = document.getElementById('podcast-audio');

audioElement.addEventListener("play", function() {
  var button = document.querySelector("#podcast-play-button");
  changeButtonToPause(button);
});

audioElement.addEventListener("pause", function() {
  var button = document.querySelector("#podcast-play-button");
  changeButtonToPlay(button);
});


document.querySelector("#podcast-play-button").addEventListener("click", function(){
  var audio = document.getElementById('podcast-audio');

  if(this.className === "play-button is-playing"){
    changeButtonToPlay(this);
    audio.pause();
  } else{
    changeButtonToPause(this);
    audio.play();
  }

});

</script>
{RAW/}

<br>
[![Try out RavenDB 4.2 for Free](images/try-out-rdb42.png)](https://ravendb.net/downloads)
