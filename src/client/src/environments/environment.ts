// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { IEnviromnent } from './IEnvironment';
import { LogLevel } from '@microsoft/signalr';

export const environment: IEnviromnent = {
	production: false,
	api: {
		url: 'http://localhost:5000/api'
	},
	signalr: {
		url: 'http://localhost:5000/hubs/game',
		logLevel: LogLevel.Debug
	}
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
import 'zone.js/dist/zone-error';  // Included with Angular CLI.
