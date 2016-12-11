public function BotonJugar(){
	
	Application.LoadLevel("AlphaLevel");
	
}

public function BotonMenu(){
	
    Debug.Log("Change");
    Application.LoadLevel("AlphaMenu");
	
}

public function BotonSalir(){
	
	Application.Quit();
	
}