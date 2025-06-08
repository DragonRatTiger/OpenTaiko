using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTaiko {
	public class CLuaCharacterScript {
		private static string backup_script = """
			local Character = CHARA:LoadChara()

			function init()
				EVENT:Subscribe("Legacy_AnimationRequested")
			end

			function update()
			end

			function draw()
			end

			function TriggerEvent(string, lua_event)
				if string == "Legacy_AnimationRequested" then
					Character[lua_event.SubName][lua_event.Name][lua_event.Frame]:SetScale(lua_event.Scale_X, lua_event.Scale_Y)
					Character[lua_event.SubName][lua_event.Name][lua_event.Frame]:DrawAtAnchor(lua_event.X, lua_event.Y, lua_event.Anchor)
				end
			end
			""";
	}
}
