export class LoginInformacije {
  autentifikacijaToken?: AutentifikacijaToken |null=null;
  isLogiran: boolean=false;
}

export interface AutentifikacijaToken {
  id: number;
  vrijednost: string;
  korisnikId: number;
  korisnik: Korisnik;
  vrijemeEvidentiranja: string;
}

export interface Korisnik {
  id: number;
  username: string;
  slika: string;
  isStudent: boolean;
  isAdmin: boolean;
  isReferentFakulteta: boolean;
  isReferentUniverziteta: boolean;
  isIzdavacSmjestaja: boolean;
  isUposlenikFirme: boolean;
  isUposlenikStipenditora: boolean;
}
