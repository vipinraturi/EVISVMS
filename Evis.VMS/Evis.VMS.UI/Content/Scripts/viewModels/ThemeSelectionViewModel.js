function ThemeSelectionViewModel() {
    var self = this;
  
    self.Theme1 = function () {
        //alert("Applying Theme1");
        changetheme("theme1");
    }

    self.Theme2 = function () {
        //alert("Applying theme2");
        changetheme("theme2");
    }

    self.Theme3 = function () {
        //alert("Applying theme3");
        changetheme("theme3");
    }

    self.Theme4 = function () {
        //alert("Applying theme4");
        changetheme("theme4");
    }
}