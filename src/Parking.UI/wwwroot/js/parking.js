var parking = (function () {
  'use strict';

  function In() {

    try {
      document.getElementById("btnIn").disabled = true;

      document.getElementById("Type").value = "In";

      document.getElementById("TagNumber").removeAttribute("disabled");

      var form = $('#frmInOut');
      form.removeData("validator");
      $.validator.unobtrusive.parse(form);

      if (form.valid() && form.data('validator').pendingRequest == 0) {
        var uri = baseUrl + "/Parking/In";

        fetch(uri, {
          method: 'POST',
          headers: {
            'Accept': 'application/x-www-form-urlencoded',
            'Content-Type': 'application/x-www-form-urlencoded'
          },
          body: $("#frmInOut").serialize()
        })
          .then((response) => {
            return response.text();

          })
          .then((result) => {
            document.getElementById("inOutView").innerHTML = result;
            Spots();
          })
          .catch(error => {
            //TODO: Log the error
            throw error;
          });
      }
    }
    catch (ex) {
      alert('There is an issue occured processing your request.Contact your administrator.');
    }
    finally {
      document.getElementById("btnIn").disabled = false;
    }
  }

  function Out() {

    try {

      document.getElementById("btnOut").disabled = true;

      if (document.getElementById("Type").value != "OutValidated") {
        document.getElementById("Type").value = "Out";
      }

      var form = $('#frmInOut');
      form.removeData("validator");
      $.validator.unobtrusive.parse(form);

      if (form.valid() && form.data('validator').pendingRequest == 0) {

        document.getElementById("TagNumber").removeAttribute("disabled");

        var uri = baseUrl + "/Parking/Out";

        fetch(uri, {
          method: 'POST',
          headers: {
            'Accept': 'application/x-www-form-urlencoded',
            'Content-Type': 'application/x-www-form-urlencoded'
          },
          body: $("#frmInOut").serialize()
        })
          .then((response) => {
            return response.text();
          })
          .then((result) => {
            document.getElementById("inOutView").innerHTML = result;
            Spots();
          })
          .catch(error => {
            //TODO: Log the error
            throw error;
          });
      }
    }
    catch (ex) {
      alert('There is an issue occured processing your request.Contact your administrator.');
    }
    finally {
      document.getElementById("btnOut").disabled = false;
    }
  }

  function Stats() {

    try {

      var uri = baseUrl + "/Parking/Stats";

      fetch(uri, {
        method: 'POST',
        headers: {
          'Accept': 'application/x-www-form-urlencoded',
        }
      })
        .then((response) => {
          return response.text();
        })
        .then((result) => {

          document.getElementById("modalBody").innerHTML = result;
          $('#statsModal').modal('show');

        })
        .catch(error => {
          //TODO: Log the error
          throw error;
        });
    }
    catch (ex) {
      alert('There is an issue occured processing your request.Contact your administrator.');
    }
  }
 
  function SpotsDeprecated() {
    /*
    var uri = baseUrl + "/Parking/Spots";

    fetch(uri, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
      }
    })
      .then((response) => {
          return response.json();
      })
      .then((result) => {

        document.getElementById('tblBody').innerHTML = ""; 

        var row = ""; 
        for (var index in result.parkIns) {

          var parkin = result.parkIns[index];

          row = row + "<tr><td>";
          row = row + parkin.tagNumber;
          row = row + "</td><td>";
          var parsedDate = new Date(parkin.checkIn);
          row = row + parsedDate.toLocaleString([], { hour12: true }).replace(","," ");
          row = row + "</td><td>";

          var delta = Math.abs(new Date(Date.now()) - parsedDate) / 1000;

          // calculate (and subtract) whole days
          var days = Math.floor(delta / 86400);
          delta -= days * 86400;

          // calculate (and subtract) whole hours
          var hours = Math.floor(delta / 3600) % 24;
          delta -= hours * 3600;

          // calculate (and subtract) whole minutes
          var minutes = Math.floor(delta / 60) % 60;
          delta -= minutes * 60;

          // what's left is seconds
          var seconds = Math.floor(delta % 60);

          row = row + hours + ":" + minutes + ":" + seconds;
          row = row + "</td></tr>";
        }
        document.getElementById('tblBody').innerHTML = row;

        document.getElementById("spotTaken").innerText = result.spotTaken;
        var totalSpot = parseInt(document.getElementById("totalSpot").innerText);
        var availableSport = totalSpot - parseInt(result.spotTaken);
        document.getElementById("spotAvailable").innerText = availableSport;

      })
      .catch(error => {
        alert('Unable to Load Spot.Please contact to admin.');
      });
      */
  }

  function Spots() {

    var uri = baseUrl + "/Parking/Spots";

    fetch(uri, {
      method: 'POST',
      headers: {
        'Accept': 'application/x-www-form-urlencoded',
      }
    })
      .then((response) => {
        return response.text();
      })
      .then((result) => {

        document.getElementById("splotList").innerHTML = result;

      })
      .catch(error => {
        //TODO: Log the error
        throw error;
      });
  }

  function Reset() {

    var uri = baseUrl + "/Parking/In";

    fetch(uri, {
      method: 'GET',
      headers: {
        'Accept': 'application/x-www-form-urlencoded'
      }
     })
      .then((response) => {
        return response.text();

      })
      .then((result) => {
        document.getElementById("inOutView").innerHTML = result;
        document.getElementById("TagNumber").focus();
      })
      .catch(error => {
        //TODO: Log the error
        throw error;
      });
  }

  return {
    In: In,
    Out: Out,
    Stats: Stats,
    Reset: Reset
  };
})();